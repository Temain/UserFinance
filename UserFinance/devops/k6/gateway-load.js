import encoding from 'k6/encoding';
import http from 'k6/http';
import { check, group, sleep } from 'k6';

const baseUrl = getEnvValue('K6_BASE_URL', 'http://localhost:8080').replace(/\/$/, '');
const favoriteCurrencyIds = getEnvValue('K6_FAVORITE_CURRENCY_IDS', '840,978')
  .split(',')
  .map(value => Number(value.trim()))
  .filter(Number.isFinite);

export const options = {
  vus: Number(getEnvValue('K6_VUS', '5')),
  duration: getEnvValue('K6_DURATION', '30s'),
  thresholds: {
    http_req_failed: ['rate<0.05'],
    http_req_duration: ['p(95)<1000'],
  },
};

export function setup() {
  const suffix = `${Date.now()}-${Math.floor(Math.random() * 100000)}`;
  const credentials = {
    name: `k6-user-${suffix}`,
    password: 'K6-password-123',
  };

  const registerResponse = http.post(
    `${baseUrl}/api/users`,
    JSON.stringify(credentials),
    jsonRequestParams());

  check(registerResponse, {
    'register succeeded': response => response.status === 200,
  });

  const registerToken = extractAccessToken(registerResponse, 'register');
  const userId = extractUserId(registerToken);

  const addFavoritesResponse = http.post(
    `${baseUrl}/api/users/${userId}/favorites`,
    JSON.stringify({ favoriteCurrencyIds }),
    authorizedJsonRequestParams(registerToken));

  const favoritesSeeded = check(addFavoritesResponse, {
    'favorites seeded': response => response.status === 204,
  });

  if (!favoritesSeeded) {
    logHttpFailure('favorites seeded', addFavoritesResponse);
  }

  return {
    credentials,
    userId,
  };
}

export default function (setupData) {
  group('authenticate', () => {
    const loginResponse = http.post(
      `${baseUrl}/api/auth/login`,
      JSON.stringify(setupData.credentials),
      jsonRequestParams());

    const loginSucceeded = check(loginResponse, {
      'login succeeded': response => response.status === 200,
    });

    if (!loginSucceeded) {
      logHttpFailure('login succeeded', loginResponse);
    }

    const accessToken = extractAccessToken(loginResponse, 'login');
    const authParams = authorizedJsonRequestParams(accessToken);

    group('authorized reads', () => {
      const responses = http.batch([
        ['GET', `${baseUrl}/api/users/${setupData.userId}`, null, authParams],
        ['GET', `${baseUrl}/api/users/${setupData.userId}/favorites`, null, authParams],
        ['GET', `${baseUrl}/api/users/${setupData.userId}/favorites/rates`, null, authParams],
        ['GET', `${baseUrl}/health`, null, { tags: { name: 'health' } }],
      ]);

      const profileReadSucceeded = check(responses[0], {
        'profile read succeeded': response => response.status === 200,
      });

      if (!profileReadSucceeded) {
        logHttpFailure('profile read succeeded', responses[0]);
      }

      const favoritesReadSucceeded = check(responses[1], {
        'favorites read succeeded': response => response.status === 200,
      });

      if (!favoritesReadSucceeded) {
        logHttpFailure('favorites read succeeded', responses[1]);
      }

      const ratesReadSucceeded = check(responses[2], {
        'rates read succeeded': response => response.status === 200,
      });

      if (!ratesReadSucceeded) {
        logHttpFailure('rates read succeeded', responses[2]);
      }

      const healthCheckSucceeded = check(responses[3], {
        'health check succeeded': response => response.status === 200,
      });

      if (!healthCheckSucceeded) {
        logHttpFailure('health check succeeded', responses[3]);
      }
    });

    group('logout and revoke validation', () => {
      const logoutResponse = http.post(`${baseUrl}/api/auth/logout`, null, authParams);

      const logoutSucceeded = check(logoutResponse, {
        'logout succeeded': response => response.status === 204,
      });

      if (!logoutSucceeded) {
        logHttpFailure('logout succeeded', logoutResponse);
      }

      const revokedTokenResponse = http.get(
        `${baseUrl}/api/users/${setupData.userId}/favorites/rates`,
        withExpectedStatuses(authParams, [401]));

      const revokedTokenRejected = check(revokedTokenResponse, {
        'revoked token rejected': response => response.status === 401,
      });

      if (!revokedTokenRejected) {
        logHttpFailure('revoked token rejected', revokedTokenResponse);
      }
    });
  });

  sleep(Number(getEnvValue('K6_SLEEP_SECONDS', '1')));
}

function jsonRequestParams() {
  return {
    headers: {
      'Content-Type': 'application/json',
    },
  };
}

function authorizedJsonRequestParams(accessToken) {
  const requestParams = jsonRequestParams();
  requestParams.headers.Authorization = `Bearer ${accessToken}`;

  return {
    headers: requestParams.headers,
  };
}

function extractAccessToken(response, operationName) {
  const payload = response.json();
  const accessToken = payload && payload.accessToken;

  if (!accessToken) {
    throw new Error(`Missing accessToken in ${operationName} response.`);
  }

  return accessToken;
}

function getEnvValue(name, fallback) {
  const value = __ENV[name];
  return value === undefined || value === null || value === '' ? fallback : value;
}

function logHttpFailure(checkName, response) {
  console.error(
    `[${checkName}] status=${response.status} body=${truncate(response.body, 500)}`);
}

function withExpectedStatuses(requestParams, expectedStatuses) {
  return {
    headers: requestParams.headers,
    responseCallback: http.expectedStatuses.apply(http, expectedStatuses),
  };
}

function truncate(value, maxLength) {
  if (!value) {
    return '';
  }

  return value.length > maxLength
    ? `${value.substring(0, maxLength)}...`
    : value;
}

function extractUserId(accessToken) {
  const payloadSegment = accessToken.split('.')[1];

  if (!payloadSegment) {
    throw new Error('JWT payload segment is missing.');
  }

  const payload = JSON.parse(encoding.b64decode(payloadSegment, 'rawurl', 's'));
  const userId = Number(payload.sub);

  if (!Number.isFinite(userId)) {
    throw new Error('JWT does not contain a numeric sub claim.');
  }

  return userId;
}
