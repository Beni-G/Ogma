import { Component, computed, inject, numberAttribute, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { ItemTypeService } from '../../services/item-type.service';
import { CreateItemTypeCommand, ItemType, UpdateItemTypeCommand } from '../../models/item-type.models';

@Component({
  selector: 'app-item-types-list',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TableModule,
    ButtonModule,
    DialogModule,
    InputTextModule,
    IconFieldModule,
    InputIconModule
  ],
  templateUrl: './item-types-list.component.html',
  styleUrl: './item-types-list.component.scss'
})
export class ItemTypesListComponent implements OnInit {

  private readonly itemTypeService = inject(ItemTypeService);
  private readonly formBuilder = inject(FormBuilder);

  itemTypes = signal<ItemType[]>([]);
  dialogVisible = signal(false);
  editingItem = signal<ItemType | null>(null);
  readonly searchQuery = signal<string>('');

  readonly filteredItemTypes = computed(() => {
    const query = this.searchQuery().toLowerCase().trim();

    if (!query) {
      return this.itemTypes();
    }

    return this.itemTypes().filter(itemType =>
      itemType.name.toLowerCase().includes(query) ||
      itemType.description.toLowerCase().includes(query)
    );
  });

  form = this.formBuilder.group({
    name: ['', [Validators.required, Validators.minLength(3)]],
    description: ['', [Validators.required]]
  })

  ngOnInit(): void {
    this.loadItemTypes();
  }

  loadItemTypes(): void {
    this.itemTypeService.getAll().subscribe((data) => this.itemTypes.set(data));
  }

  openNew(): void {
    this.editingItem.set(null);
    this.form.reset();
    this.dialogVisible.set(true);
  }

  openEdit(itemType: ItemType): void {
    this.editingItem.set(itemType);
    this.form.patchValue({ name: itemType.name, description: itemType.description });
    this.dialogVisible.set(true);
  }

  onSearch(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.searchQuery.set(value);
  }

  save(): void {

    if (this.form.invalid) {
      return;
    }

    const raw = this.form.getRawValue();
    const current = this.editingItem();

    if (current) {
      const command: UpdateItemTypeCommand = { id: current.id, name: raw.name!, description: raw.description! };
      this.itemTypeService.update(command).subscribe(() => this.onSaveSuccess());
    } else {
      const command: CreateItemTypeCommand = { name: raw.name!, description: raw.description! };
      this.itemTypeService.create(command).subscribe(() => this.onSaveSuccess());
    }
  }

  delete(id: number): void {
    this.itemTypeService.delete(id).subscribe(() => this.loadItemTypes());
  }

  private onSaveSuccess(): void {
    this.dialogVisible.set(false);
    this.loadItemTypes();
  }
}

