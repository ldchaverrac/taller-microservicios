# Taller Microservicios — E-commerce

## Levantar los servicios

Se requiere Docker y Docker Compose instalados. Desde el directorio raíz del repositorio:

```bash
docker compose up --build
```

El flag `--build` solo es necesario la primera vez o tras modificar el código fuente. Para detener:

```bash
docker compose down
```

Todos los endpoints se consumen a través del API Gateway en `http://localhost:4000`.

---

## Endpoints (vía API Gateway)

### Usuarios — `/api/users`

| Método | Ruta | Cuerpo |
|--------|------|--------|
| `POST` | `/api/users` | `{ "name": "...", "email": "...", "password": "..." }` |
| `GET`  | `/api/users/:id` | — |

### Productos — `/api/products`

| Método | Ruta | Cuerpo |
|--------|------|--------|
| `POST`  | `/api/products` | `{ "name": "...", "description": "...", "price": 0.0, "stock": 0 }` |
| `GET`   | `/api/products` | — (retorna solo productos con stock > 0) |
| `PATCH` | `/api/products/:id/stock` | `{ "quantity": n }` (descuenta n unidades del stock) |

### Pedidos — `/api/orders`

| Método   | Ruta | Cuerpo |
|----------|------|--------|
| `POST`   | `/api/orders` | `{ "userId": "...", "items": [{ "productId": 1, "quantity": 1 }] }` |
| `GET`    | `/api/orders` | — |
| `GET`    | `/api/orders/:id` | — |
| `DELETE` | `/api/orders/:id` | — |

Al crear un pedido, el servicio valida la existencia del usuario y del producto, calcula el total y descuenta el stock automáticamente.

---

## Estructura de microservicios

### API Gateway — puerto 4000
- **Framework:** ASP.NET Core 9 + Ocelot 24
- **Función:** enruta todas las peticiones entrantes hacia el microservicio correspondiente. No posee base de datos ni lógica de negocio propia.

### users-service — puerto 3000
- **Framework:** NestJS 11 (Node.js)
- **Base de datos:** MongoDB (mongoose)
- **Endpoints internos:** `POST /users`, `GET /users/:id`

### products-service — puerto 5000
- **Framework:** NestJS 11 (Node.js)
- **Base de datos:** PostgreSQL 15 (TypeORM)
- **Endpoints internos:** `POST /products`, `GET /products`, `PATCH /products/:id/stock`

### orders-service — puerto 7000
- **Framework:** ASP.NET Core 9 (C#)
- **Base de datos:** SQL Server 2022 (Entity Framework Core 9)
- **Endpoints internos:** `POST /orders`, `GET /orders`, `GET /orders/:id`, `DELETE /orders/:id`
- **Dependencias en tiempo de ejecución:** consulta users-service y products-service a través del gateway para validar usuario, obtener precios y actualizar stock.