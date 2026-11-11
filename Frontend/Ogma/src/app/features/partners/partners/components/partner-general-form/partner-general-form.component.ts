import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { MultiSelectModule } from 'primeng/multiselect';
import { CheckboxModule } from 'primeng/checkbox';
import { PartnerRoleType } from '../../../partner-role-types/models/partner-role-type.models';

@Component({
  selector: 'app-partner-general-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, InputTextModule, SelectModule, MultiSelectModule, CheckboxModule],
  templateUrl: './partner-general-form.component.html'
})
export class PartnerGeneralFormComponent {
  @Input() parentForm!: FormGroup;
  @Input() roleTypes: PartnerRoleType[] = [];
  @Input() isEdit: boolean = false;
}