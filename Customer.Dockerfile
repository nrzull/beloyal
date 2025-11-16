FROM node:22.21.1-alpine AS build

ARG API_WEB_PUBLIC_URL

WORKDIR /src
COPY package.json package-lock.json .npmrc ./
COPY frontend/customer ./frontend/customer
COPY frontend/shared ./frontend/shared

RUN --mount=type=ssh npm ci
RUN npm run build -w @beloyal/customer

FROM nginx:1.29.3-alpine AS final

COPY nginx.conf /etc/nginx/conf.d/default.conf
RUN mkdir -p /usr/share/nginx/html/customer
COPY --from=build /src/frontend/customer/dist /usr/share/nginx/html/customer
COPY --from=build /src/frontend/customer/dist/index.html /usr/share/nginx/html/
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
