import { BadRequestException, Injectable, NotFoundException } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { MoreThan, Repository } from 'typeorm';
import { CreateProductDto } from './dto/create-product.dto';
import { Product } from './entities/product.entity';

@Injectable()
export class ProductsService {
  constructor(@InjectRepository(Product) private productsRepository: Repository<Product>) {}

  async create(createProductDto: CreateProductDto): Promise<Product> {
    const newProduct = this.productsRepository.create(createProductDto);
    return this.productsRepository.save(newProduct);
  }

  async findAvailable(): Promise<Product[]> {
    return this.productsRepository.find({
      where: { stock: MoreThan(0) }
    });
  }

  async updateStock(id: number, quantityToDeduct: number): Promise<Product> {
    const searchParams = { id: id };
    const searchCondition = { where: searchParams };
    const product = await this.productsRepository.findOne(searchCondition);
    if (!product) throw new NotFoundException(`Producto con ID ${id} no encontrado`);
    if (product.stock < quantityToDeduct) throw new BadRequestException(`Stock actual: ${product.stock}, solicitado: ${quantityToDeduct}`);
    product.stock -= quantityToDeduct;
    return this.productsRepository.save(product);
  }
}
