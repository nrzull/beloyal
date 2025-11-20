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
  docker compose pull
  npm ci

@caddy:
  cat Caddyfile > /etc/caddy/Caddyfile
  caddy fmt --overwrite
  caddy reload

@deploy:
  ssh $DEPLOY_SSH 'cd beloyal && git pull --ff-only && just deploy-local'

@deploy-local:
  git pull
  docker compose pull
  docker compose up -d
  docker compose --profile deploy build api
  docker compose --profile deploy build api-migrations
  docker compose --profile deploy build office
  docker compose --profile deploy build customer
  docker compose --profile deploy build agent
  docker compose --profile deploy build links
  docker compose --profile deploy run api-migrations
  docker compose --profile deploy down api office customer agent links
  docker compose --profile deploy up --remove-orphans -d

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

@dev:
  npx concurrently -n api,office,customer,agent,links,proxy4000 \
  "just api dotnet run" \
  "just office npm run dev" \
  "API_WEB_PUBLIC_URL=https://proxy5000.beloyal.ru just customer npm run dev" \
  "just agent npm run dev" \
  "just links npm run dev" \
  "bore local 4000 --to proxy4000.beloyal.ru --port 4000" \
  "bore local 5000 --to proxy5000.beloyal.ru --port 5000"

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
