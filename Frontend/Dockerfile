FROM  --platform=linux/amd64 node:18 AS base

WORKDIR /app

COPY package.json .
COPY package-lock.json .
RUN npm i --loglevel=verbose
COPY . . 

FROM base AS dev
EXPOSE 5173
RUN apt-get update && apt-get install -y nginx
CMD ["sh", "-c", "npm run dev & nginx -g 'daemon off;'"]

FROM base AS build
RUN npm run build

FROM nginx AS prod
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;" ]
