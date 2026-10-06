import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConfirmationService, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { TagModule } from 'primeng/tag';

import { CreateItemCommand, Item, UpdateItemCommand } from '../../models/item.models';
import { Category } from '../../../categories/models/category.models';
import { ItemType } from '../../../item-types/models/item-type.models';

import { ItemService } from '../../services/item.service';
import { ColumnDefinition, ItemsTableComponent } from '../../components/items-table/items-table.component';
import { ItemFormDialogComponent, ItemFormValue } from '../../components/item-form-dialog/item-form-dialog.component';
import { CategoryFilterComponent } from '../../components/category-filter/category-filter.component';

@Component({
  selector: 'app-items-list',
  standalone: true,
  imports: [
    CommonModule,
    ButtonModule,
    ConfirmDialogModule,
    ToastModule,
    TagModule,
    CategoryFilterComponent,
    ItemsTableComponent,
    ItemFormDialogComponent
  ],
  providers: [ConfirmationService, MessageService],
  templateUrl: './items-list.component.html',
  styleUrl: './items-list.component.scss'
})
export class ItemsListComponent implements OnInit {
  private readonly itemService = inject(ItemService);
  private readonly confirmationService = inject(ConfirmationService);
  private readonly messageService = inject(MessageService);

  allItems: Item[] = [];
  items: Item[] = [];

  categories: Category[] = [];
  itemTypes: ItemType[] = [];

  activeCategoryId: number | null = null;
  activeBranchIds: number[] = [];

  dialogVisible = false;
  editingItem: Item | null = null;

  readonly columns: ColumnDefinition[] = [
    {
      field: 'code',
      header: 'Code',
      visible: true,
      sortable: true
    },
    {
      field: 'name',
      header: 'Name',
      visible: true,
      sortable: true
    },
    {
      field: 'category',
      header: 'Category Path',
      visible: true,
      sortable: false
    },
    {
      field: 'itemType.name',
      header: 'Item Type',
      visible: true,
      sortable: true
    },
    {
      field: 'listPrice.amount',
      header: 'Price',
      visible: true,
      sortable: true
    },
    {
      field: 'unitOfMeasurement',
      header: 'UOM',
      visible: true,
      sortable: true
    },
    {
      field: 'isActive',
      header: 'Status',
      visible: true,
      sortable: true
    }
  ];

  ngOnInit(): void {
    this.loadLookups();
    this.loadItems();
  }

  private loadLookups(): void {
    this.itemService.getCategories().subscribe({
      next: categories => {
        this.categories = categories;
      },
      error: () => {
        this.showError('Failed to load categories.');
      }
    });

    this.itemService.getItemTypes().subscribe({
      next: itemTypes => {
        this.itemTypes = itemTypes;
      },
      error: () => {
        this.showError('Failed to load item types.');
      }
    });
  }

  private loadItems(): void {
    this.itemService.getAll().subscribe({
      next: items => {
        this.allItems = items;
        this.applyCategoryFilter();
      },
      error: () => {
        this.showError('Failed to load items.');
      }
    });
  }

  onCategoryChange(event: {
    categoryId: number | null;
    branchIds: number[];
  }): void {
    this.activeCategoryId = event.categoryId;
    this.activeBranchIds = event.branchIds;

    this.applyCategoryFilter();
  }

  private applyCategoryFilter(): void {
    if (this.activeCategoryId === null) {
      this.items = [...this.allItems];
      return;
    }

    this.items = this.allItems.filter(item =>
      item.categoryId != null &&
      this.activeBranchIds.includes(item.categoryId)
    );
  }

  openNewItem(): void {
    this.editingItem = null;
    this.dialogVisible = true;
  }

  editItem(item: Item): void {
    this.editingItem = item;
    this.dialogVisible = true;
  }

  saveItem(value: ItemFormValue): void {
    if (value.categoryId === null || value.itemTypeId === null) {
      this.showError('Category and item type are required.');
      return;
    }

    const command: CreateItemCommand = {
      code: value.code,
      name: value.name,
      description: value.description,
      categoryId: value.categoryId,
      itemTypeId: value.itemTypeId,
      unitOfMeasurement: value.unitOfMeasurement,
      isActive: value.isActive,
      listPrice: {
        amount: value.amount,
        currency: value.currency
      }
    };

    const request =
      value.id !== null
        ? this.itemService.update({
          ...command,
          id: value.id
        } satisfies UpdateItemCommand)
        : this.itemService.create(command);

    request.subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: value.id !== null ? 'Updated' : 'Created',
          detail: 'Item saved successfully'
        });

        this.dialogVisible = false;
        this.editingItem = null;

        this.loadItems();
      },
      error: () => {
        this.showError('Failed to save the item.');
      }
    });
  }

  deleteItem(item: Item): void {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete "${item.name}" (${item.code})?`,
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      acceptButtonStyleClass: 'p-button-danger p-button-sm',
      rejectButtonStyleClass: 'p-button-secondary p-button-text p-button-sm',

      accept: () => {
        this.itemService.delete(item.id).subscribe({
          next: () => {
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'Item deleted successfully'
            });

            this.loadItems();
          },
          error: () => {
            this.showError('Failed to delete the item.');
          }
        });
      }
    });
  }

  private showError(detail: string): void {
    this.messageService.add({
      severity: 'error',
      summary: 'Error',
      detail
    });
  }
}