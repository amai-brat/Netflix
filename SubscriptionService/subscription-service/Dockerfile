FROM node:20

WORKDIR /app

COPY package.json .
COPY package-lock.json .
RUN npm i --loglevel verbose

COPY . .

EXPOSE 3000

CMD ["npm", "run", "start:dev"]
