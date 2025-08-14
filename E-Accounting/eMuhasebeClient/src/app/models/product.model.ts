import { ProductDetailModel } from "./product-detail.model";

export class ProductModel{
     
    id: string = "";
    name: string = "";
    deposit: number = 0;
    withDrawal: number = 0;
    details: ProductDetailModel[] = [];
}