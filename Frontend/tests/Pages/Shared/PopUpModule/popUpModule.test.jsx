import { render, screen, fireEvent } from '@testing-library/react';
import { describe, test, expect, beforeEach } from "vitest";
import ComponentWithPopUp from "../../../../src/Pages/Shared/PopUpModule/ComponentWithPopUp.jsx";

const id = 'test-popup';

const MockComponent = () => <div>Mock Component</div>;
    
const MockPopUp = ({ setPopUpDisplayed }) => (
    <div>
        <div>Mock PopUp</div>
        <button onClick={() => setPopUpDisplayed(false)}>Close</button>
    </div>
);

const setup = () => {
    render(
        <ComponentWithPopUp Component={MockComponent} PopUp={MockPopUp} id={id} />
    );
};

describe('ComponentWithPopUp', () => {
    beforeEach(() => {
        setup();
    });
    
    test('DefaultDisplaysCorrect', () => {
        expect(screen.getByText(/Mock Component/i)).toBeInTheDocument();
        expect(screen.queryByText(/Mock PopUp/i)).not.toBeVisible();
    });
    
    test('DisplaysThePopUpWhenTheComponentIsClicked', () => {
        expect(screen.queryByText(/Mock PopUp/i)).not.toBeVisible();
        
        fireEvent.click(screen.getByText(/Mock Component/i));
        
        expect(screen.getByText(/Mock PopUp/i)).toBeVisible();
    });

    test('HidesThePopUpWhenClickingOutsideOfIt', () => {
        fireEvent.click(screen.getByText(/Mock Component/i));
        expect(screen.getByText(/Mock PopUp/i)).toBeVisible();
        
        fireEvent.mouseDown(document.body);

        expect(screen.queryByText(/Mock PopUp/i)).not.toBeVisible();
    });

    test('HidesThePopUpWhenTheCloseButtonIsClicked', () => {
        fireEvent.click(screen.getByText(/Mock Component/i));
        expect(screen.getByText(/Mock PopUp/i)).toBeVisible();

        fireEvent.click(screen.getByText(/Close/i));

        expect(screen.queryByText(/Mock PopUp/i)).not.toBeVisible();
    });
});
