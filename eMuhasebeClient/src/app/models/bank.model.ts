import { BankDetailModel } from "./bank-detail.model";
import { CurrencyTypeModel } from "./currency-type.model";

export class BankModel{
    
    id: string = "";
    name: string = "";
    iban: string = "";
    depositAmount: number = 0;
    withDrawalAmount: number = 0;
    currencyType : CurrencyTypeModel = new CurrencyTypeModel();
    currencyTypeValue: number = 1;
    details: BankDetailModel[] = [];

}