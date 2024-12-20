import {fireEvent, render, screen} from '@testing-library/react';
import { describe, test, expect } from "vitest";
import ClientPanel from "../../../../src/Pages/Shared/Header/ClientPanel.jsx";

const defaultIcon = '/src/assets/default.png';

const mockUser = {
    icon: 'https://example.com/icon.jpg'
}

const setup = (user) => {
    render(
        <ClientPanel user={user} />
    );
};

describe('ClientPanel', () => {
    test('RendersUserIconWhenUserIconIsProvided', () => {
        setup(mockUser)
        const imgElement = screen.getByAltText(/UserIcon/i);
        expect(imgElement).toHaveAttribute('src', mockUser.icon);
    });

    test('RendersDefaultIconWhenNoUserIconIsProvided', () => {
        setup({})
        const imgElement = screen.getByAltText(/UserIcon/i);
        expect(imgElement).toHaveAttribute('src', defaultIcon);
    });

    test('RendersDefaultIconWhenUserIconFailsToLoad', () => {
        setup(mockUser)
        const imgElement = screen.getByAltText(/UserIcon/i);
        expect(imgElement).toHaveAttribute('src', mockUser.icon);
        
        fireEvent.error(imgElement)
        
        expect(imgElement).toHaveAttribute('src', defaultIcon);
    });
});
