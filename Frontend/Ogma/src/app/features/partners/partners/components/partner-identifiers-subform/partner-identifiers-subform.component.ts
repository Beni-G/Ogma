import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';

@Component({
  selector: 'app-partner-identifiers-subform',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ButtonModule, InputTextModule, CheckboxModule],
  templateUrl: './partner-identifiers-subform.component.html'
})
export class PartnerIdentifiersSubformComponent {
  private fb = inject(FormBuilder);
  @Input() parentForm!: FormGroup;

  get identifiers(): FormArray { 
    return this.parentForm.get('identifiers') as FormArray; 
  }

  onPrimaryChange(changedIndex: number) {
    this.identifiers.controls.forEach((ctrl, idx) => {
      if (idx !== changedIndex) {
        ctrl.get('isPrimary')?.setValue(false, { emitEvent: false });
      }
    });
  }

  add(data?: any) {
    const formatDate = (dateStr?: string | null) => {
      if (!dateStr) return '';
      return dateStr.split('T')[0];
    };

    this.identifiers.push(
      this.fb.group({
        id: [data?.id ?? 0],
        type: [data?.type ?? '', Validators.required],
        value: [data?.value ?? '', Validators.required],
        validityPeriod: this.fb.group({
          start: [formatDate(data?.validityPeriod?.start)],
          end: [formatDate(data?.validityPeriod?.end)]
        }),
        isPrimary: [data?.isPrimary ?? (this.identifiers.length === 0)]
      })
    );
  }

  remove(index: number) { 
    this.identifiers.removeAt(index); 
  }
}