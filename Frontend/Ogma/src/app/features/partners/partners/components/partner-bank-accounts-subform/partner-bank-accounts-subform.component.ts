import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';

@Component({
  selector: 'app-partner-bank-accounts-subform',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ButtonModule, InputTextModule, CheckboxModule],
  templateUrl: './partner-bank-accounts-subform.component.html'
})
export class PartnerBankAccountsSubformComponent {
  private fb = inject(FormBuilder);
  @Input() parentForm!: FormGroup;

  get bankAccounts(): FormArray {
    return this.parentForm.get('bankAccounts') as FormArray;
  }

  add(data?: any) {
    this.bankAccounts.push(
      this.fb.group({
        id: [data?.id ?? 0],
        isDefault: [data?.isDefault ?? false],
        bankAccount: this.fb.group({
          bank: [data?.bankAccount?.bank ?? ''],
          iban: [data?.bankAccount?.iban ?? '', Validators.required],
          currency: [data?.bankAccount?.currency ?? 'EUR'],
          bic: [data?.bankAccount?.bic ?? '']
        })
      })
    );
  }

  remove(index: number) {
    this.bankAccounts.removeAt(index);
  }
}