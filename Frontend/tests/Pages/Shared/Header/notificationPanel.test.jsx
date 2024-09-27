import { render, screen } from '@testing-library/react';
import NotificationPanel from "../../../../src/Pages/Shared/Header/NotificationPanel.jsx";

const setup = (alarmed) => {
    render(
        <NotificationPanel alarmed={alarmed} />
    );
};

describe('NotificationPanel', () => {
    test('RendersNotificationIconWhenNotAlarmed', () => {
        setup(false)
        const notificationIcon = screen.getByAltText('Notification');
        const alarmedIcon = screen.queryByAltText(/NotificationAlarm/i);

        expect(notificationIcon).toBeVisible();
        expect(alarmedIcon).not.toBeVisible();
    });

    test('RendersAlarmedIconWhenAlarmed', () => {
        setup(true)
        const notificationIcon = screen.queryByAltText('Notification');
        const alarmedIcon = screen.getByAltText(/NotificationAlarm/i);

        expect(alarmedIcon).toBeVisible();
        expect(notificationIcon).not.toBeVisible();
    });
});
