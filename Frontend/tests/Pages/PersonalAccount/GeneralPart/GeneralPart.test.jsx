import { describe, it, expect, beforeEach, vi } from 'vitest';
import { render, screen } from '@testing-library/react';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import GeneralPart from '../../../../src/Pages/PersonalAccount/GeneralPart/GeneralPart.jsx';
import { authenticationService } from '../../../../src/services/authentication.service';
import userEvent from '@testing-library/user-event';

vi.mock('../../../../src/services/authentication.service', () => ({
    authenticationService: {
        getUser: vi.fn(),
    },
}));

describe('GeneralPart Component', () => {
    const tabs = [
        { name: "Личные данные", link: "PersonalInfoTab" },
        { name: "Избранное", link: "FavouritesTab" },
        { name: "Рецензии", link: "PersonalReviewsTab" },
        { name: "Подписки", link: "SubscriptionsTab" },
        { name: "Контент", link: "admin/content" },
        { name: "Подписки управление", link: "admin/subscriptions" }
    ];

    beforeEach(() => {
        vi.resetAllMocks();
        authenticationService.getUser.mockReturnValue({
            id: 1,
            name: 'Test User',
            role: "support, admin"
        });
    });

    const renderComponent = (initialRoute = '/PersonalAccount/PersonalInfoTab') => {
        return render(
            <MemoryRouter initialEntries={[initialRoute]}>
                <Routes>
                    <Route path="/PersonalAccount/*" element={<GeneralPart />}>
                        {/* Вложенные маршруты */}
                        <Route path="PersonalInfoTab" element={<div>Personal Info Content</div>} />
                        <Route path="FavouritesTab" element={<div>Favourites Content</div>} />
                        <Route path="PersonalReviewsTab" element={<div>Personal Reviews Content</div>} />
                        <Route path="SubscriptionsTab" element={<div>Subscriptions Content</div>} />
                        <Route path="admin/content" element={<div>Admin Content</div>} />
                        <Route path="admin/subscriptions" element={<div>Admin Subscriptions</div>} />
                    </Route>
                </Routes>
            </MemoryRouter>
        );
    };

    it('должен рендерить все вкладки', () => {
        renderComponent();
        tabs.forEach(tab => {
            expect(screen.getByText(tab.name)).toBeInTheDocument();
        });
    });


    it('должен переключаться на вкладку при клике', async () => {
        renderComponent();

        const user = userEvent.setup();

        const reviewsTab = screen.getByText('Рецензии');
        await user.click(reviewsTab);

        expect(screen.getByText('Personal Reviews Content')).toBeInTheDocument();
    });

    it('должен устанавливать начальную вкладку при изменении маршрута', () => {
        const { rerender } = render(
            <MemoryRouter initialEntries={['/PersonalAccount/PersonalInfoTab']}>
                <Routes>
                    <Route path="/PersonalAccount/*" element={<GeneralPart />}>
                        <Route path="PersonalInfoTab" element={<div>Personal Info Content</div>} />
                        <Route path="FavouritesTab" element={<div>Favourites Content</div>} />
                    </Route>
                </Routes>
            </MemoryRouter>
        );


        rerender(
            <MemoryRouter initialEntries={['/PersonalAccount/FavouritesTab']}>
                <Routes>
                    <Route path="/PersonalAccount/*" element={<GeneralPart />}>
                        <Route path="PersonalInfoTab" element={<div>Personal Info Content</div>} />
                        <Route path="FavouritesTab" element={<div>Favourites Content</div>} />
                    </Route>
                </Routes>
            </MemoryRouter>
        );

    });

    it('должен отображать Outlet', () => {
        renderComponent();
        expect(screen.getByText('Personal Info Content')).toBeInTheDocument();
    });

    it('должен вызывать authenticationService.getUser при рендере', () => {
        renderComponent();
        expect(authenticationService.getUser).toHaveBeenCalled();
    });
});
