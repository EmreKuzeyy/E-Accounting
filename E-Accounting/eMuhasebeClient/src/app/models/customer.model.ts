import { CustomerDetailModel } from "./customer-detail.model";

export class CustomerModel{
   
    id: string = "";
    name: string = "";
    type: CustomerTypeEnum = new CustomerTypeEnum();
    typeValue: number = 1;
    city: string = "";
    town: string = "";
    taxDepartment: string = "";
    taxNumber: string = "";
    fullAddress: string = "";
    depositAmount: number = 0;
    withDrawalAmount: number = 0;
    details: CustomerDetailModel[]= [];
}


export class CustomerTypeEnum{
    
    name: string = "";
    value: number = 1;

}

export const CustomerTypes: CustomerTypeEnum[] = [

   {
     name: "Ticari Alıcılar",
     value: 1
 
   },
   {
     name: "Ticari Satıcılar",
     value: 2
 
   },
   {
     name: "Personel",
     value: 3
 
   },
   {
     name: "Ortaklar",
     value: 4
 
   },

]