import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';

@Component({
  selector: 'app-partner-contacts-subform',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ButtonModule, InputTextModule, CheckboxModule],
  templateUrl: './partner-contacts-subform.component.html'
})
export class PartnerContactsSubformComponent {
  private fb = inject(FormBuilder);
  @Input() parentForm!: FormGroup;

  get contacts(): FormArray {
    return this.parentForm.get('contacts') as FormArray;
  }

  add(data?: any) {
    this.contacts.push(
      this.fb.group({
        id: [data?.id ?? 0],
        isPrimary: [data?.isPrimary ?? false],
        email: [data?.email ?? ''],
        phone: [data?.phone ?? ''],
        mobile: [data?.mobile ?? ''],
        title: [data?.title ?? ''],
        jobTitle: [data?.jobTitle ?? ''],
        name: this.fb.group({
          firstName: [data?.name?.firstName ?? '', Validators.required],
          lastName: [data?.name?.lastName ?? '', Validators.required]
        })
      })
    );
  }

  remove(index: number) {
    this.contacts.removeAt(index);
  }
}