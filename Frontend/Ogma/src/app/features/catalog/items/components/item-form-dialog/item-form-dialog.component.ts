import { Component, input, output, effect, inject, untracked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { InputNumberModule } from 'primeng/inputnumber';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { FloatLabelModule } from 'primeng/floatlabel';

import { Item } from '../../models/item.models';
import { Category } from '../../../categories/models/category.models';
import { ItemType } from '../../../item-types/models/item-type.models';


export interface ItemFormValue {
  id: number | null;
  code: string;
  name: string;
  description: string;
  categoryId: number | null;
  itemTypeId: number | null;
  amount: number;
  currency: string;
  unitOfMeasurement: string;
  isActive: boolean;
}

@Component({
  selector: 'app-item-form-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DialogModule,
    ButtonModule,
    InputTextModule,
    SelectModule,
    InputNumberModule,
    ToggleSwitchModule,
    FloatLabelModule
  ],
  templateUrl: './item-form-dialog.component.html'
})
export class ItemFormDialogComponent {
  private readonly fb = inject(FormBuilder);

  visible = input(false);
  item = input<Item | null>(null);
  categories = input<Category[]>([]);
  itemTypes = input<ItemType[]>([]);
  defaultCategoryId = input<number | null>(null);

  visibleChange = output<boolean>();
  save = output<ItemFormValue>();

  readonly form = this.fb.group({
    id: this.fb.control<number | null>(null),
    code: ['', Validators.required],
    name: ['', Validators.required],
    description: [''],
    categoryId: this.fb.control<number | null>(
      null,
      Validators.required
    ),
    itemTypeId: this.fb.control<number | null>(
      null,
      Validators.required
    ),
    amount: [0, [Validators.required, Validators.min(0)]],
    currency: ['RON', Validators.required],
    unitOfMeasurement: ['buc', Validators.required],
    isActive: [true]
  });

  constructor() {
    effect(() => {
      const isVisible = this.visible();

      if (!isVisible) {
        return;
      }

      untracked(() => {
        const item = this.item();

        if (item) {
          this.form.reset({
            id: item.id,
            code: item.code,
            name: item.name,
            description: item.description ?? '',
            categoryId: item.categoryId,
            itemTypeId: item.itemTypeId,
            amount: item.listPrice?.amount ?? 0,
            currency: item.listPrice?.currency ?? 'RON',
            unitOfMeasurement: item.unitOfMeasurement ?? 'buc',
            isActive: item.isActive
          });
        } else {
          this.form.reset({
            id: null,
            code: '',
            name: '',
            description: '',
            categoryId: this.defaultCategoryId(),
            itemTypeId: null,
            amount: 0,
            currency: 'RON',
            unitOfMeasurement: 'buc',
            isActive: true
          });
        }
      });
    });
  }

  get isEditMode(): boolean {
    return this.item() !== null;
  }

  close(): void {
    this.visibleChange.emit(false);
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.save.emit(this.form.getRawValue() as ItemFormValue);
  }
}