set dotenv-load
export PATH := "./node_modules/.bin:" + env_var('PATH')

@api *args:
  cd backend/api && {{args}}

@api-test *args:
  cd backend/api-test && {{args}}

@db *args:
  cd backend/api && dotnet ef {{args}}

@db-reset:
  just api dotnet ef database drop --force
  just api dotnet ef database update
  API_USE_SEEDING=1 just api dotnet ef database update

@office *args:
  cd frontend/office && {{args}}

@customer *args:
  cd frontend/customer && {{args}}

@devops:
  docker compose up -d

@ci:
  dotnet restore
  dotnet tool restore
  npm ci

@dev-local:
  npx concurrently -n api,office,customer \
  "just api dotnet run" \
  "just office npm run dev" \
  "just customer npm run dev"

@dev:
  API_WEB_PUBLIC_URL=https://nrzull.beloyal.ru npx concurrently -n api,office,customer,officebore,customerbore,apibore \
  "just api dotnet run" \
  "just office npm run dev" \
  "just customer npm run dev" \
  "bore local 3000 --to nrzull.beloyal.ru --port 3000" \
  "bore local 3001 --to nrzull.beloyal.ru --port 3001" \
  "bore local 8080 --to nrzull.beloyal.ru --port 8080"

@build:
  just api dotnet build
  just api-test dotnet build
  just office npm run build

@test:
  POSTGRES_DATABASE=beloyal_test just api dotnet ef database drop --force
  POSTGRES_DATABASE=beloyal_test just api dotnet ef database update
  POSTGRES_DATABASE=beloyal_test API_USE_SEEDING=1 just api dotnet ef database update
  ASPNETCORE_ENVIRONMENT=Development POSTGRES_DATABASE=beloyal_test just api-test dotnet test

@retest:
  ASPNETCORE_ENVIRONMENT=Development POSTGRES_DATABASE=beloyal_test just api-test dotnet test

# screen -dmS bore bash -c 'bore server --secret $BORE_SECRET'
# screen -ls
# screen -r bore
# Ctrl+A D
