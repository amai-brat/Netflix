FROM node as build

WORKDIR /app

COPY package.json .
COPY package-lock.json .
RUN npm i

COPY . . 

RUN npm run build

FROM nginx as runtime

COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;" ]
