set dotenv-load
export PATH := "./node_modules/.bin:" + env_var('PATH')

# screen -dmS bore bash -c 'bore server --secret $BORE_SECRET'
# screen -ls
# screen -r bore
# Ctrl+A D

@devops:
  docker compose up -d

@ci:
  dotnet restore
  dotnet tool restore
  npm ci

@api *args:
  cd backend/api && {{args}}

@api-test *args:
  cd backend/api-test && {{args}}

@db *args:
  cd backend/api && dotnet ef {{args}}

@db-reset:
  just api dotnet ef database drop --force
  just api dotnet ef database update

@office *args:
  cd frontend/office && {{args}}

@customer *args:
  cd frontend/customer && {{args}}

@agent *args:
  cd frontend/agent && {{args}}

@links *args:
  cd frontend/links && {{args}}

@dev-local:
  npx concurrently -n api,office,customer,agent \
  "just api dotnet run" \
  "just office npm run dev" \
  "just customer npm run dev" \
  "just agent npm run dev" \
  "just links npm run dev"

@dev-remote:
  RUSTFS_PUBLIC_URL=https://s3.beloyal.ru \
  CUSTOMER_WEB_PUBLIC_URL=https://dev.beloyal.ru/customer \
  API_WEB_PUBLIC_URL=https://dev.beloyal.ru \
  LINKS_WEB_PUBLIC_URL=https://dev.beloyal.ru/links \
  npx concurrently -n api,office,customer,agent,links,apibore,officebore,customerbore,agentbore,linksbore,s3bore \
  "just api dotnet run" \
  "just office npm run dev" \
  "just customer npm run dev" \
  "just agent npm run dev" \
  "just links npm run dev" \
  "bore local 8080 --to dev.beloyal.ru --port 8080" \
  "bore local 3000 --to dev.beloyal.ru --port 3000" \
  "bore local 3001 --to dev.beloyal.ru --port 3001" \
  "bore local 3002 --to dev.beloyal.ru --port 3002" \
  "bore local 3003 --to dev.beloyal.ru --port 3003" \
  "bore local 9000 --to s3.beloyal.ru --port 9000"

@build-backend:
  just api dotnet build
  just api-test dotnet build

@build-frontend:
  just office npm run build
  just customer npm run build
  just agent npm run build
  just links npm run build

@build:
  just build-backend
  just build-frontend

@test:
  POSTGRES_DATABASE=beloyal_test just api dotnet ef database drop --force
  POSTGRES_DATABASE=beloyal_test just api dotnet ef database update
  ASPNETCORE_ENVIRONMENT=Test POSTGRES_DATABASE=beloyal_test just api-test dotnet test

@retest:
  ASPNETCORE_ENVIRONMENT=Test POSTGRES_DATABASE=beloyal_test just api-test dotnet test

@office-dockerfile-build:
  export DOCKER_BUILDKIT=1
  docker build -t office-app --ssh default --build-arg API_WEB_PUBLIC_URL --build-arg LINKS_WEB_PUBLIC_URL -f Office.Dockerfile .

@agent-dockerfile-build:
  export DOCKER_BUILDKIT=1
  docker build -t agent-app --ssh default --build-arg API_WEB_PUBLIC_URL -f Agent.Dockerfile .

@links-dockerfile-build:
  export DOCKER_BUILDKIT=1
  docker build -t links-app --ssh default --build-arg API_WEB_PUBLIC_URL -f Links.Dockerfile .

@customer-dockerfile-build:
  export DOCKER_BUILDKIT=1
  docker build -t customer-app --ssh default --build-arg API_WEB_PUBLIC_URL -f Customer.Dockerfile .

@frontend-dockerfile-build:
  just office-dockerfile-build
  just agent-dockerfile-build
  just links-dockerfile-build
  just customer-dockerfile-build

@backend-dockerfile-build:
  export DOCKER_BUILDKIT=1
  docker build -t api --ssh default -f Api.Dockerfile .
  docker build -t api-migrations --ssh default -f ApiMigrations.Dockerfile .

@dockerfile-build:
  just backend-dockerfile-build
  just frontend-dockerfile-build
