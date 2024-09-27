import { render, screen } from '@testing-library/react';
import { describe, test, expect } from "vitest";
import SelectionContentGrid from "../../../src/Pages/SelectionContent/SelectionContentGrid.jsx";

vi.mock("../../../src/Pages/SelectionContent/SelectionContentCard.jsx", () => {
    return {
        __esModule: true,
        default: ({ content }) => <div>{content.title}</div>,
    };
});

const mockContents = [
    { title: 'Контент 1' },
    { title: 'Контент 2' },
    { title: 'Контент 3' },
];

const setup = (contents) => {
    render(<SelectionContentGrid contents={contents} />);
}

describe('SelectionContentGrid', () => {

    test('RendersNoContentMessageWhenContentsIsEmpty', () => {
        setup([])
        
        expect(screen.getByText(/Ничего не найдено/i)).toBeInTheDocument();
    });

    test('RendersContentCardsWhenContentsIsNotEmpty', () => {
        setup(mockContents)

        expect(screen.queryByLabelText(/Ничего не найдено/i)).not.toBeInTheDocument();
        expect(screen.getByText('Контент 1')).toBeInTheDocument();
        expect(screen.getByText('Контент 2')).toBeInTheDocument();
        expect(screen.getByText('Контент 3')).toBeInTheDocument();
    });
});
