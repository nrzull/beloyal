FROM node:22.21.1-alpine AS build

ARG API_WEB_PUBLIC_URL
ARG LINKS_WEB_PUBLIC_URL

WORKDIR /src
COPY . .

RUN --mount=type=ssh npm ci
RUN npm run build -w @beloyal/office

FROM nginx:1.29.3-alpine AS final

COPY nginx.conf /etc/nginx/conf.d/default.conf
RUN mkdir -p /usr/share/nginx/html/office
COPY --from=build /src/frontend/office/dist /usr/share/nginx/html/office
COPY --from=build /src/frontend/office/dist/index.html /usr/share/nginx/html/
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
