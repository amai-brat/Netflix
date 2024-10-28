import {render, screen, fireEvent, waitFor} from '@testing-library/react';
import { describe, test, vi, expect } from "vitest";
import { BrowserRouter } from 'react-router-dom';
import ClientPopUpPanel from "../../../../src/Pages/Shared/Header/ClientPopUpPanel.jsx";
import {authenticationService} from "../../../../src/services/authentication.service.js";
import {DataStoreProvider} from "../../../../src/store/dataStoreProvider.jsx";

vi.mock("../../../../src/services/authentication.service.js");

const setPopUpDisplayed = vi.fn();

const mockUser = {
    name: 'John Doe'
}

const setup = (user) => {
    render(
        <DataStoreProvider>
            <BrowserRouter>
                <ClientPopUpPanel user={user} setPopUpDisplayed={setPopUpDisplayed} />
            </BrowserRouter>
        </DataStoreProvider>
    );
};

describe('ClientPopUpPanel', () => {
    test('DisplaysNavCorrect', () => {
        setup(mockUser)

        expect(screen.getByText(/Личные данные/i)).toBeInTheDocument();
        expect(screen.getByText(/Рецензии/i)).toBeInTheDocument();
        expect(screen.getByText(/Избранное/i)).toBeInTheDocument();
        expect(screen.getByText(/Подписки/i)).toBeInTheDocument();
    });
    
    test('DisplaysUserNameWhenUserIsProvided', () => {
        setup(mockUser)
        
        expect(screen.getByText(mockUser.name)).toBeInTheDocument();
    });

    test('DoesNotDisplayUserNameWhenUserIsNull', () => {
        setup(null)
        
        expect(screen.queryByText(mockUser.name)).not.toBeInTheDocument();
    });

    test('NavigatesToPersonalAccountOnLabelClick', () => {
        setup(mockUser)
        
        fireEvent.click(screen.getByText(/Личные данные/i));

        expect(window.location.pathname).toBe(`/PersonalAccount/PersonalInfoTab`);
        expect(setPopUpDisplayed).toHaveBeenCalledWith(false);
    });

    test('NavigatesToPersonalReviewsTabOnLabelClick', () => {
        setup(mockUser)

        fireEvent.click(screen.getByText(/Рецензии/i));

        expect(window.location.pathname).toBe(`/PersonalAccount/PersonalReviewsTab`);
        expect(setPopUpDisplayed).toHaveBeenCalledWith(false);
    });

    test('NavigatesToFavouritesTabOnLabelClick', () => {
        setup(mockUser)

        fireEvent.click(screen.getByText(/Избранное/i));

        expect(window.location.pathname).toBe(`/PersonalAccount/FavouritesTab`);
        expect(setPopUpDisplayed).toHaveBeenCalledWith(false);
    });

    test('NavigatesToSubscriptionsTabOnLabelClick', () => {
        setup(mockUser)

        fireEvent.click(screen.getByText(/Подписки/i));

        expect(window.location.pathname).toBe(`/PersonalAccount/SubscriptionsTab`);
        expect(setPopUpDisplayed).toHaveBeenCalledWith(false);
    });
    
    test('HandlesLogoutButtonClick', async () => {
        setup(mockUser)

        fireEvent.click(screen.getByRole('button', { name: /Выйти/i }));
        
        expect(authenticationService.logout).toHaveBeenCalled();
        
        await waitFor(() => {
            expect(window.location.pathname).toBe(`/`);
        });
    });
});
