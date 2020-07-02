/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { ApplicationsComponent } from './applications.component';

let component: ApplicationsComponent;
let fixture: ComponentFixture<ApplicationsComponent>;

describe('applications component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ ApplicationsComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(ApplicationsComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});