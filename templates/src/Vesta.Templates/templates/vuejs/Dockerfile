FROM node:14-alpine as build-stage
RUN apk add --no-cache python3 g++ make
# hacer la carpeta 'app' el directorio de trabajo actual
WORKDIR /app
# copiar 'package.json' y 'package-lock.json' (si están disponibles)
COPY package*.json ./
# TODO: Tmp, copiamos la template de env para poder desplegar como development
COPY .env.template ./.env
# instalar dependencias del proyecto
RUN npm install
# copiar los archivos y carpetas del proyecto al directorio de trabajo actual (es decir, la carpeta 'app')
COPY . .
# construir aplicación para producción minificada
RUN npm run build

# PRODUCCIÓN
FROM nginx:stable-alpine as production-stage
COPY --from=build-stage /app/dist /usr/share/nginx/html
COPY nginx/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]

