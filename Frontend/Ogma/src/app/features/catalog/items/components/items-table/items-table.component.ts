import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MultiSelectModule } from 'primeng/multiselect';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';

import { Item } from '../../models/item.models';
import { Category } from '../../../categories/models/category.models';

export interface ColumnDefinition {
  field: string;
  header: string;
  visible: boolean;
  sortable: boolean;
}

@Component({
  selector: 'app-items-table',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    MultiSelectModule,
    TagModule,
    TooltipModule
  ],
  templateUrl: './items-table.component.html'
})
export class ItemsTableComponent {
  items = input<Item[]>([]);
  columns = input<ColumnDefinition[]>([]);

  edit = output<Item>();
  delete = output<Item>();

  selectedColumns: ColumnDefinition[] = [];

  ngOnChanges(): void {
    const available = this.columns();

    this.selectedColumns = available.filter(column =>
      this.selectedColumns.some(selected =>
        selected.field === column.field
      ) || column.visible
    );
  }

  onColumnsChange(columns: ColumnDefinition[]): void {
    const available = this.columns();

    this.selectedColumns = available.filter(column =>
      columns.some(selected =>
        selected.field === column.field
      )
    );
  }

  getBreadcrumbPath(category: Category | null | undefined): string {
    if (!category) {
      return '—';
    }

    const value = category as any;

    if (value.ancestors?.length) {
      const names = value.ancestors.map(
        (ancestor: Category) => ancestor.name
      );

      return [...names, category.name].join(' > ');
    }

    return category.name;
  }
}