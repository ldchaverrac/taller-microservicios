import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { UsersModule } from './users/users.module';
import { MongooseModule } from '@nestjs/mongoose';

function env(name: string): string {
  const value = process.env[name];
  if (!value) throw new Error(`Variable de entorno requerida: ${name}`);
  return value;
}

@Module({
  imports: [
    UsersModule,
    MongooseModule.forRoot(env('DB_URI'), { dbName: env('DB_NAME') })
  ],
  controllers: [AppController],
  providers: [AppService]
})
export class AppModule {}