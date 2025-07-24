set dotenv-load
export PATH := "./node_modules/.bin:" + env_var('PATH')

@api *args:
  cd backend/api && {{args}}

@api-test *args:
  cd backend/api-test && {{args}}

@db *args:
  cd backend/api && dotnet ef {{args}}

@devops:
  docker compose up -d

@ci:
  dotnet restore
  npm ci

@build:
  just api dotnet build
  just api-test dotnet build

@test:
  POSTGRES_DATABASE=beloyal_test just api dotnet ef database drop --force
  POSTGRES_DATABASE=beloyal_test just api dotnet ef database update
  POSTGRES_DATABASE=beloyal_test API_USE_SEEDING=1 just api dotnet ef database update
  POSTGRES_DATABASE=beloyal_test just api-test dotnet test

@retest:
  POSTGRES_DATABASE=beloyal_test just api-test dotnet test
