import {render, screen} from '@testing-library/react';
import { describe, test, expect, beforeEach } from "vitest";
import {BrowserRouter} from 'react-router-dom';
import NotificationPopUpPanel from "../../../../src/Pages/Shared/Header/NotificationPopUpPanel.jsx";
import {DataStoreProvider} from "../../../../src/store/dataStoreProvider.jsx";

const notificationsMock = [
    {
        id: 1,
        comment: {
            user: { nickname: 'User1' },
            text: 'This is a comment',
            writtenAt: '2023-10-01T12:00:00Z',
            review: { contentId: 101 },
        },
    },
    {
        id: 2,
        comment: {
            user: { nickname: 'User2' },
            text: 'Another comment',
            writtenAt: '2023-10-02T12:00:00Z',
            review: { contentId: 102 },
        },
    },
];

const setup = () => {
    render(
        <DataStoreProvider>
            <BrowserRouter>
                <NotificationPopUpPanel notifications={notificationsMock} />
            </BrowserRouter>
        </DataStoreProvider>
    );
};

describe('NotificationPopUpPanel', () => {
    beforeEach(() => {
        setup();
    });
    
    test('RendersNotificationsCorrectly', () => {
        expect(screen.getByText('Ответ от User1: This is a comment')).toBeInTheDocument();
        expect(screen.getByText('2023-10-01')).toBeInTheDocument();
        expect(screen.getByText('Ответ от User2: Another comment')).toBeInTheDocument();
        expect(screen.getByText('2023-10-02')).toBeInTheDocument();
    });
});
