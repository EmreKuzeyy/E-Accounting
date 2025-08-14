import { BankModel } from "./bank.model";
import { CashRegisterModel } from "./cash-register.model";

export class CashRegisterDetailModel{
    id: string = "";
    cashRegisterId: string = "";
    date: string = "";
    type: number = 0;
    amount: number = 0;
    depositAmount: number = 0;
    withDrawalAmount: number = 0;
    cashRegisterDetailId: string = "";
    bankDetailId: string = "";
    customerDetailId: string = "";
    oppositeCustomerId: string | any = "";
    oppositeCashRegisterId: string | any = "";
    oppositeCashRegister: CashRegisterModel = new CashRegisterModel();
    oppositeBankId: string | any = "";
    oppositeBank: BankModel = new BankModel(); 
    description: string = "";
    oppositeAmount: number = 0;
    recordType: number = 0;
}