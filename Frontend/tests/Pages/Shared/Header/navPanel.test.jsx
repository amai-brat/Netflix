import { render, screen, fireEvent } from '@testing-library/react';
import { describe, test, expect, beforeEach } from "vitest";
import {BrowserRouter} from 'react-router-dom';
import NavigatePanel from "../../../../src/Pages/Shared/Header/NavigatePanel.jsx";

const setup = () => {
    render(
        <BrowserRouter>
            <NavigatePanel />
        </BrowserRouter>
    );
};

describe('NavigatePanel', () => {
    beforeEach(() => {
        setup();
    });

    test('NavigatesToMainContentOnLogoClick', () => {
        fireEvent.click(screen.getByAltText(/Voltorka/i));
        expect(window.location.pathname).toBe(`/MainContent`);
    });

    test('NavigatesToSelectionContentForMoviesOnLabelClick', () => {
        fireEvent.click(screen.getByText('Фильмы'));
        expect(window.location.pathname).toBe(`/SelectionContent`);
    });

    test('NavigatesToSelectionContentForCartoonsOnLabelClick', () => {
        fireEvent.click(screen.getByText(/Мультфильмы/i));
        expect(window.location.pathname).toBe(`/SelectionContent`);
    });

    test('NavigatesToSelectionContentForSeriesOnLabelClick', () => {
        fireEvent.click(screen.getByText(/Сериалы/i));
        expect(window.location.pathname).toBe(`/SelectionContent`);
    });
});
