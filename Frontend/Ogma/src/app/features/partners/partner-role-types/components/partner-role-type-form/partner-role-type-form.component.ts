import { Component, Input, OnInit, Output, EventEmitter, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { ColorPickerModule } from 'primeng/colorpicker';
import { MessageService } from 'primeng/api';
import { PartnerRoleTypeService } from '../../services/partner-role-type.service';
import { PartnerRoleType } from '../../models/partner-role-type.models';


@Component({
  selector: 'app-partner-role-type-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ButtonModule, InputTextModule, ColorPickerModule],
  templateUrl: './partner-role-type-form.component.html'
})
export class PartnerRoleTypeFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private service = inject(PartnerRoleTypeService);
  private messageService = inject(MessageService);

  @Input() role: PartnerRoleType | null = null;
  @Output() saved = new EventEmitter<void>();
  @Output() cancelled = new EventEmitter<void>();

  form!: FormGroup;

  ngOnInit() {
    this.form = this.fb.group({
      code: [this.role?.code ?? '', [Validators.required]],
      name: [this.role?.name ?? '', [Validators.required]],
      color: [this.role?.color ?? '#3b82f6']
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    const formValue = this.form.value;

    if (this.role) {
      const updateDto: PartnerRoleType = { id: this.role.id, ...formValue };
      this.service.update(this.role.id, updateDto).subscribe({
        next: () => {
          this.saved.emit();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to update role type' });
        }
      });
    } else {
      this.service.create(formValue).subscribe({
        next: () => {
          this.saved.emit();
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to create role type' });
        }
      });
    }
  }
}