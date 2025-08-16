set dotenv-load
export PATH := "./node_modules/.bin:" + env_var('PATH')

@api *args:
  cd backend/api && {{args}}

@api-test *args:
  cd backend/api-test && {{args}}

@db *args:
  cd backend/api && dotnet ef {{args}}

@db-update:
  just api dotnet ef database drop --force
  just api dotnet ef database update
  API_USE_SEEDING=1 just api dotnet ef database update

@office *args:
  cd frontend/office && {{args}}

@devops:
  docker compose up -d

@ci:
  dotnet restore
  npm ci

@dev:
  npx concurrently -n api,office "just api dotnet run" "just office npm run dev"

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
