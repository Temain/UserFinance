# k6

Базовый сценарий лежит в [gateway-load.js](/Users/temain/Projects/UserFinance/UserFinance/devops/k6/gateway-load.js).

Что делает сценарий:
- в `setup()` регистрирует уникального пользователя через gateway
- логинится
- один раз добавляет favorite currencies
- в каждом iteration делает:
  - `login`
  - `GET /api/users/{userId}`
  - `GET /api/users/{userId}/favorites`
  - `GET /api/users/{userId}/favorites/rates`
  - `POST /api/auth/logout`
  - повторный запрос с отозванным токеном и ожидает `401`

Запуск через docker compose:

```bash
docker compose --profile load-testing run --rm k6
```

Полезные переменные:
- `K6_VUS`
- `K6_DURATION`
- `K6_SLEEP_SECONDS`
- `K6_FAVORITE_CURRENCY_IDS`

Пример:

```bash
K6_VUS=10 K6_DURATION=1m docker compose --profile load-testing run --rm k6
```
