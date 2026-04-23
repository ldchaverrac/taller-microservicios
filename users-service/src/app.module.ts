import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { UsersModule } from './users/users.module';
import { MongooseModule } from '@nestjs/mongoose';

@Module({
  imports: [
    UsersModule,
    MongooseModule.forRoot('mongodb://admin:SuperSecretPassword123!@localhost:27017', {
      dbName: 'users_db'
    })
  ],
  controllers: [AppController],
  providers: [AppService]
})
export class AppModule {}
