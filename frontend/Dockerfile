FROM node:20-alpine AS build

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run build --omit=dev


FROM nginx:alpine

COPY --from=build /app/dist/task-management/browser /usr/share/nginx/html

RUN rm /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/conf.d/nginx.conf

CMD ["nginx", "-g", "daemon off;"]
