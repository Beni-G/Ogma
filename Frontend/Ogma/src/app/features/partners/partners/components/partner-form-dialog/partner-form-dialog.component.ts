import { Component, Input, OnInit, Output, EventEmitter, inject, signal, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { TabsModule } from 'primeng/tabs';
import { MessageService } from 'primeng/api';
import { Partner, CreatePartnerRequest, UpdatePartnerRequest } from '../../models/partner.models';
import { PartnerService } from '../../services/partner.service';
import { PartnerRoleType } from '../../../partner-role-types/models/partner-role-type.models';
import { PartnerRoleTypeService } from '../../../partner-role-types/services/partner-role-type.service';
import { PartnerGeneralFormComponent } from '../partner-general-form/partner-general-form.component';
import { PartnerAddressFormComponent } from '../partner-address-form/partner-address-form.component';
import { PartnerIdentifiersSubformComponent } from '../partner-identifiers-subform/partner-identifiers-subform.component';
import { PartnerBankAccountsSubformComponent } from '../partner-bank-accounts-subform/partner-bank-accounts-subform.component';
import { PartnerContactsSubformComponent } from '../partner-contacts-subform/partner-contacts-subform.component';

@Component({
  selector: 'app-partner-form-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ButtonModule,
    TabsModule,
    PartnerGeneralFormComponent,
    PartnerAddressFormComponent,
    PartnerIdentifiersSubformComponent,
    PartnerBankAccountsSubformComponent,
    PartnerContactsSubformComponent
  ],
  templateUrl: './partner-form-dialog.component.html'
})
export class PartnerFormDialogComponent implements OnInit, AfterViewInit {
  private fb = inject(FormBuilder);
  private partnerService = inject(PartnerService);
  private roleTypeService = inject(PartnerRoleTypeService);
  private messageService = inject(MessageService);

  @Input() partner: Partner | null = null;
  @Output() saved = new EventEmitter<void>();
  @Output() cancelled = new EventEmitter<void>();

  @ViewChild(PartnerIdentifiersSubformComponent) identifiersSubform?: PartnerIdentifiersSubformComponent;
  @ViewChild(PartnerBankAccountsSubformComponent) bankAccountsSubform?: PartnerBankAccountsSubformComponent;
  @ViewChild(PartnerContactsSubformComponent) contactsSubform?: PartnerContactsSubformComponent;

  form!: FormGroup;
  roleTypes = signal<PartnerRoleType[]>([]);

  ngOnInit() {
    const isEdit = !!this.partner;

    const initialRoleIds = this.partner?.roles 
      ? this.partner.roles.map(r => Number(r.id)) 
      : [];

    this.form = this.fb.group({
      isNaturalPerson: [this.partner?.isNaturalPerson ?? true],
      isActive: [this.partner?.isActive ?? true],
      companyName: [this.partner?.companyName ?? ''],
      individualName: this.fb.group({
        firstName: [this.partner?.individualName?.firstName ?? ''],
        lastName: [this.partner?.individualName?.lastName ?? '']
      }),
      hqAddress: this.fb.group({
        street: [this.partner?.hqAddress?.street ?? ''],
        number: [this.partner?.hqAddress?.number ?? ''],
        city: [this.partner?.hqAddress?.city ?? ''],
        region: [this.partner?.hqAddress?.region ?? ''],
        postalCode: [this.partner?.hqAddress?.postalCode ?? ''],
        countryCode: [this.partner?.hqAddress?.countryCode ?? ''],
        building: [this.partner?.hqAddress?.building ?? ''],
        stairCase: [this.partner?.hqAddress?.stairCase ?? ''],
        floor: [this.partner?.hqAddress?.floor ?? ''],
        apartment: [this.partner?.hqAddress?.apartment ?? '']
      }),
      roleId: [isEdit ? null : (initialRoleIds[0] ?? null), isEdit ? [] : [Validators.required]],
      roleIds: [initialRoleIds],
      identifier: this.fb.group({
        type: ['VAT', isEdit ? [] : [Validators.required]],
        value: ['', isEdit ? [] : [Validators.required]],
        validityPeriod: this.fb.group({
          start: [null],
          end: [null]
        }),
        isPrimary: [true]
      }),
      identifiers: this.fb.array([]),
      bankAccounts: this.fb.array([]),
      contacts: this.fb.array([])
    });

    this.setupValidationTriggers();
    this.loadRoleTypes();
  }

  ngAfterViewInit() {
    if (this.partner) {
      this.partner.identifiers?.forEach(i => this.identifiersSubform?.add(i));
      this.partner.bankAccounts?.forEach(b => this.bankAccountsSubform?.add(b));
      this.partner.contacts?.forEach(c => this.contactsSubform?.add(c));
    }
  }

  setupValidationTriggers() {
    const isNatCtrl = this.form.get('isNaturalPerson');
    const updateValidators = (isNat: boolean) => {
      const indGroup = this.form.get('individualName') as FormGroup;
      const firstNameCtrl = indGroup.get('firstName');
      const lastNameCtrl = indGroup.get('lastName');
      const compCtrl = this.form.get('companyName');

      if (isNat) {
        firstNameCtrl?.setValidators([Validators.required]);
        lastNameCtrl?.setValidators([Validators.required]);
        compCtrl?.clearValidators();
      } else {
        compCtrl?.setValidators([Validators.required]);
        firstNameCtrl?.clearValidators();
        lastNameCtrl?.clearValidators();
      }

      // Explicitly update validity on the individual controls so their error states clear out
      firstNameCtrl?.updateValueAndValidity();
      lastNameCtrl?.updateValueAndValidity();
      
      indGroup.updateValueAndValidity();
      compCtrl?.updateValueAndValidity();
    };

    updateValidators(isNatCtrl?.value);
    isNatCtrl?.valueChanges.subscribe(updateValidators);
  }

  loadRoleTypes() {
    this.roleTypeService.getAll().subscribe({
      next: (data) => {
        this.roleTypes.set(data);
        if (this.partner?.roles) {
          const ids = this.partner.roles.map(r => Number(r.id));
          this.form.get('roleIds')?.setValue(ids);
        }
      },
      error: () => this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load role types' })
    });
  }

  onSubmit() {
    if (this.form.invalid) return;
    const val = this.form.value;

    const formatValidity = (vp: any) => {
      if (!vp) return undefined;
      const start = vp.start && vp.start.trim() !== '' ? new Date(vp.start).toISOString() : null;
      const end = vp.end && vp.end.trim() !== '' ? new Date(vp.end).toISOString() : null;
      if (!start && !end) return undefined;
      return { start, end };
    };

    if (this.partner) {
      const updateDto: UpdatePartnerRequest = {
        id: this.partner.id,
        isNaturalPerson: val.isNaturalPerson,
        isActive: val.isActive,
        companyName: val.isNaturalPerson ? null : val.companyName,
        individualName: val.isNaturalPerson ? val.individualName : null,
        hqAddress: val.hqAddress,
        roleIds: val.roleIds, // Array of IDs sent to the update DTO
        identifiers: val.identifiers.map((i: any) => ({
          id: i.id,
          type: i.type,
          value: i.value,
          isPrimary: i.isPrimary,
          validityPeriod: formatValidity(i.validityPeriod)
        })),
        bankAccounts: val.bankAccounts,
        contacts: val.contacts
      };

      this.partnerService.update(this.partner.id, updateDto).subscribe({
        next: () => this.saved.emit(),
        error: () => this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to update partner' })
      });
    } else {
      const createDto: CreatePartnerRequest = {
        isNaturalPerson: val.isNaturalPerson,
        companyName: val.isNaturalPerson ? undefined : val.companyName,
        individualName: val.isNaturalPerson ? val.individualName : undefined,
        hqAddress: val.hqAddress,
        roleId: val.roleId,
        identifier: {
          ...val.identifier,
          validityPeriod: formatValidity(val.identifier.validityPeriod)
        }
      };

      this.partnerService.create(createDto).subscribe({
        next: () => this.saved.emit(),
        error: () => this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to create partner' })
      });
    }
  }
}