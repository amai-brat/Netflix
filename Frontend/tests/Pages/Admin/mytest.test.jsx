import { render, screen } from '@testing-library/react';
import AdminContent from '../../../src/Pages/Admin/Content/AdminContent.jsx';
import { test, expect } from 'vitest';

test('renders MyComponent', () => {
    render(<AdminContent/>);
    expect(screen.getByText('Добавить фильм')).toBeInTheDocument();
});
