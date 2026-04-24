// src/app.module.ts
import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { ProductsModule } from './products/products.module';
import { Product } from './products/entities/product.entity';

function env(name: string): string {
  const value = process.env[name];
  if (!value) throw new Error(`Variable de entorno requerida: ${name}`);
  return value;
}

@Module({
  imports: [
    ProductsModule,
    TypeOrmModule.forRoot({
      type: 'postgres',
      host: env('DB_HOST'),
      port: parseInt(env('DB_PORT')),
      username: env('DB_USER'),
      password: env('DB_PASSWORD'),
      database: env('DB_NAME'),
      entities: [Product],
      synchronize: true
    })
  ]
})
export class AppModule {}