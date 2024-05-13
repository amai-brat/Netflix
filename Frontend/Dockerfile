FROM node
EXPOSE 5173

WORKDIR /app

COPY package.json .
COPY package-lock.json .
RUN npm i

COPY . . 

ARG VITE_PORT=5173
ARG VITE_HOST=0.0.0.0

ENV VITE_PORT=${VITE_PORT}
ENV VITE_HOST=${VITE_HOST}

CMD ["npm", "run", "dev"]