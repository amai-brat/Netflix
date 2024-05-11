FROM node
EXPOSE 5173

WORKDIR /app

COPY package.json .
RUN npm i

COPY . . 

CMD ["npm", "run", "dev"]