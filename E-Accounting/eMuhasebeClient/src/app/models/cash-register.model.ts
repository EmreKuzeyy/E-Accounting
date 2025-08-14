import { CashRegisterDetailModel } from "./cash-register-detail.model";
import { CurrencyTypeModel } from "./currency-type.model";

export class CashRegisterModel{
   
    id: string = "";
    name: string = "";
    depositAmount: number = 0;
    withDrawalAmount: number = 0;
    currencyType : CurrencyTypeModel = new CurrencyTypeModel();
    currencyTypeValue: number = 1;
    details: CashRegisterDetailModel[] = [];
}