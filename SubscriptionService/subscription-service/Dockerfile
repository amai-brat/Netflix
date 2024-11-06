FROM --platform=linux/amd64 node:18

WORKDIR /app

COPY package.json .
COPY package-lock.json .
RUN npm i --loglevel silly

COPY . .

EXPOSE 3000

CMD ["npm", "run", "start:dev"]