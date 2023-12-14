FROM node:alpine
EXPOSE 4200

WORKDIR /front-app

COPY package*.json ./

RUN npm install

RUN npm rebuild esbuild 

CMD ["npm", "start"]
