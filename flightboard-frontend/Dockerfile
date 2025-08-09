# --- Build stage ---
FROM node:20-alpine AS build
WORKDIR /app

# Install deps first (better Docker cache)
COPY package.json package-lock.json* pnpm-lock.yaml* yarn.lock* ./
RUN if [ -f package-lock.json ]; then npm ci; \
    elif [ -f pnpm-lock.yaml ]; then npm i -g pnpm && pnpm i --frozen-lockfile; \
    elif [ -f yarn.lock ]; then yarn install --frozen-lockfile; \
    else npm i; fi

# Copy source and build
COPY . .
RUN npm run build

# --- Runtime stage (Nginx) ---
FROM nginx:1.27-alpine AS runtime
WORKDIR /usr/share/nginx/html
# Clean default content
RUN rm -rf ./*
# Copy built app
COPY --from=build /app/dist ./
# Nginx config with API proxy + SignalR
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
