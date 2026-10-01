import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { TreeModule } from 'primeng/tree';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DialogModule } from 'primeng/dialog';
import { SelectModule } from 'primeng/select';
import { MultiSelectModule } from 'primeng/multiselect';
import { InputNumberModule } from 'primeng/inputnumber';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService, TreeNode } from 'primeng/api';

import { Category } from '../../models/category.models';
import { Item } from '../../models/item.models';
import { ItemService } from '../../services/item.service';
import { ItemType } from '../../models/item-type.models';

export interface ColumnDefinition {
  field: string;
  header: string;
  visible: boolean;
  sortable: boolean;
}

@Component({
  selector: 'app-items-list',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TableModule,
    TreeModule,
    ButtonModule,
    InputTextModule,
    DialogModule,
    SelectModule,
    MultiSelectModule,
    InputNumberModule,
    ToggleSwitchModule,
    TagModule,
    TooltipModule,
    ConfirmDialogModule,
    ToastModule
  ],
  providers: [ConfirmationService, MessageService],
  templateUrl: './items-list.component.html',
  styleUrl: './items-list.component.scss'
})
export class ItemsListComponent implements OnInit {
  private readonly itemService = inject(ItemService);
  private readonly formBuilder = inject(FormBuilder);
  private readonly confirmationService = inject(ConfirmationService);
  private readonly messageService = inject(MessageService);

  allItems: Item[] = [];
  items: Item[] = [];

  categories: Category[] = [];
  itemTypes: ItemType[] = [];
  categoryNodes: TreeNode[] = [];

  selectedCategoryNode: TreeNode | null = null;
  activeCategoryId: number | null = null;

  itemDialog = false;
  isEditMode = false;

  cols: ColumnDefinition[] = [
    { field: 'code', header: 'Code', visible: true, sortable: true },
    { field: 'name', header: 'Name', visible: true, sortable: true },
    { field: 'category', header: 'Category Path', visible: true, sortable: false },
    { field: 'itemType.name', header: 'Item Type', visible: true, sortable: true },
    { field: 'listPrice.amount', header: 'Price', visible: true, sortable: true },
    { field: 'unitOfMeasurement', header: 'UOM', visible: true, sortable: true },
    { field: 'isActive', header: 'Status', visible: true, sortable: true }
  ];
  _selectedColumns: ColumnDefinition[] = this.cols;

  itemForm: FormGroup = this.formBuilder.group({
    id: [null],
    code: ['', Validators.required],
    name: ['', Validators.required],
    description: [''],
    categoryId: [null, Validators.required],
    itemTypeId: [null, Validators.required],
    amount: [0, [Validators.required, Validators.min(0)]],
    currency: ['RON', Validators.required],
    unitOfMeasurement: ['buc', Validators.required],
    isActive: [true]
  });

  ngOnInit(): void {
    this.loadLookups();
    this.loadItems();
  }

  get selectedColumns(): ColumnDefinition[] {
    return this._selectedColumns;
  }

  set selectedColumns(val: ColumnDefinition[]) {
    this._selectedColumns = this.cols.filter(col => val.includes(col));
  }

  loadLookups(): void {
    this.itemService.getCategories().subscribe(res => {
      this.categories = res;
      this.categoryNodes = this.buildTreeNodes(res);
    });

    this.itemService.getItemTypes().subscribe(res => {
      this.itemTypes = res;
    });
  }

  loadItems(): void {
    this.itemService.getAll().subscribe(res => {
      this.allItems = res;
      this.applyCategoryFilter();
    });
  }

  private buildTreeNodes(categories: Category[]): TreeNode[] {
    if (!categories || categories.length === 0) {
      return []
    };

    const isNested = categories.some(cat => {
      const c = cat as any;
      return (c.subCategories && c.subCategories.length > 0) || (c.children && c.children.length > 0);
    });

    if (isNested) {
      return this.mapNestedToTreeNodes(categories);
    }

    const nodeMap = new Map<number, TreeNode>();
    const rootNodes: TreeNode[] = [];

    categories.forEach(cat => {
      nodeMap.set(cat.id, {
        key: cat.id.toString(),
        label: cat.name,
        data: cat,
        expandedIcon: 'pi pi-folder-open',
        collapsedIcon: 'pi pi-folder',
        children: []
      });
    });

    categories.forEach(cat => {
      const node = nodeMap.get(cat.id)!;
      const catAny = cat as any;
      const parentId = catAny.parentId ?? catAny.parentCategoryId;

      if (parentId && nodeMap.has(parentId)) {
        nodeMap.get(parentId)!.children!.push(node);
      } else {
        rootNodes.push(node);
      }
    });

    this.cleanUpEmptyChildren(rootNodes);
    return rootNodes;
  }

  private mapNestedToTreeNodes(categories: Category[]): TreeNode[] {
    return categories.map(cat => {
      const childCategories = (cat as any).subCategories ?? (cat as any).children ?? [];
      const hasChildren = childCategories.length > 0;

      return {
        key: cat.id?.toString(),
        label: cat.name,
        data: cat,
        expandedIcon: 'pi pi-folder-open',
        collapsedIcon: 'pi pi-folder',
        icon: hasChildren ? undefined : 'pi pi-tag',
        children: hasChildren ? this.mapNestedToTreeNodes(childCategories) : []
      };
    });
  }

  private cleanUpEmptyChildren(nodes: TreeNode[]): void {
    nodes.forEach(node => {
      if (node.children && node.children.length > 0) {
        this.cleanUpEmptyChildren(node.children);
      } else {
        node.children = undefined;
        node.icon = 'pi pi-tag';
      }
    });
  }

  onCategorySelect(event: { node: TreeNode }): void {
    if (event.node && event.node.data) {
      this.activeCategoryId = event.node.data.id;
      this.selectedCategoryNode = event.node;
      this.applyCategoryFilter();
    }
  }

  onCategoryUnselect(): void {
    this.clearCategoryFilter();
  }

  clearCategoryFilter(): void {
    this.selectedCategoryNode = null;
    this.activeCategoryId = null;
    this.applyCategoryFilter();
  }

  private applyCategoryFilter(): void {
    if (!this.activeCategoryId || !this.selectedCategoryNode) {
      this.items = [...this.allItems];
      return;
    }

    const branchCategoryIds = this.getBranchCategoryIds(this.selectedCategoryNode);

    this.items = this.allItems.filter(item =>
      item.categoryId != null && branchCategoryIds.includes(item.categoryId)
    );
  }

  private getBranchCategoryIds(node: TreeNode): number[] {
    let ids: number[] = [];

    if (node.data && node.data.id) {
      ids.push(node.data.id);
    }

    if (node.children && node.children.length > 0) {
      node.children.forEach(childNode => {
        ids = ids.concat(this.getBranchCategoryIds(childNode));
      });
    }

    return ids;
  }

  openNewItem(): void {
    this.isEditMode = false;
    this.itemForm.reset({
      currency: 'RON',
      unitOfMeasurement: 'buc',
      isActive: true,
      categoryId: this.activeCategoryId
    });
    this.itemDialog = true;
  }

  editItem(item: Item): void {
    this.isEditMode = true;
    this.itemForm.patchValue({
      id: item.id,
      code: item.code,
      name: item.name,
      description: item.description,
      categoryId: item.categoryId,
      itemTypeId: item.itemTypeId,
      amount: item.listPrice?.amount ?? 0,
      currency: item.listPrice?.currency ?? 'RON',
      unitOfMeasurement: item.unitOfMeasurement,
      isActive: item.isActive
    });
    this.itemDialog = true;
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
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: 'Failed to delete the item'
            });
          }
        });
      }
    });
  }

  saveItem(): void {
    if (this.itemForm.invalid) return;

    const val = this.itemForm.value;
    const command = {
      name: val.name,
      code: val.code,
      description: val.description,
      categoryId: val.categoryId,
      itemTypeId: val.itemTypeId,
      unitOfMeasurement: val.unitOfMeasurement,
      isActive: val.isActive,
      listPrice: {
        amount: val.amount,
        currency: val.currency
      }
    };

    if (this.isEditMode) {
      this.itemService.update({ ...command, id: val.id }).subscribe(() => {
        this.messageService.add({ severity: 'success', summary: 'Updated', detail: 'Item saved successfully' });
        this.loadItems();
        this.itemDialog = false;
      });
    } else {
      this.itemService.create(command).subscribe(() => {
        this.messageService.add({ severity: 'success', summary: 'Created', detail: 'Item created successfully' });
        this.loadItems();
        this.itemDialog = false;
      });
    }
  }

  getBreadcrumbPath(category: Category): string {
    if (!category) return '—';

    const categoryAny = category as any;
    if (categoryAny.ancestors && categoryAny.ancestors.length > 0) {
      const ancestorNames = categoryAny.ancestors.map((a: Category) => a.name);
      return [...ancestorNames, category.name].join(' > ');
    }

    return category.name;
  }
}