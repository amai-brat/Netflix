import { vi, describe, it, expect, beforeEach } from 'vitest'
import React from 'react'
import { render, screen, fireEvent, waitFor } from '@testing-library/react'
import AdminContent from '../../../../src/Pages/Admin/Content/AdminContent.jsx'

vi.mock('react-toastify', () => ({
    toast: {
        error: vi.fn(),
        success: vi.fn(),
    },
}))

vi.mock('../../../../src/services/admin.content.service.js', () => ({
    adminContentService: {
        getEditMovieContent: vi.fn(),
        getEditSerialContent: vi.fn(),
        deleteContent: vi.fn(),
    },
}))

vi.mock('../../../../src/Pages/Admin/Content/AddMovieOptions.jsx', () => ({
    default: () => <div data-testid="add-movie-options">AddMovieOptions</div>,
}))

vi.mock('../../../../src/Pages/Admin/Content/AddSerialOptions.jsx', () => ({
    default: () => <div data-testid="add-serial-options">AddSerialOptions</div>,
}))

vi.mock('../../../../src/Pages/Admin/Content/EditMovieOptions', () => ({
    default: ({ movieOptions }) => (
        <div data-testid="edit-movie-options">EditMovieOptions: {JSON.stringify(movieOptions)}</div>
    ),
}))

vi.mock('../../../../src/Pages/Admin/Content/EditSerialOptions.jsx', () => ({
    default: ({ serialOptions }) => (
        <div data-testid="edit-serial-options">EditSerialOptions: {JSON.stringify(serialOptions)}</div>
    ),
}))

import { toast } from 'react-toastify'
import { adminContentService } from '../../../../src/services/admin.content.service.js'

describe('AdminContent Component', () => {
    beforeEach(() => {
        vi.resetAllMocks()
    })

    it('renders all main sections correctly', () => {
        render(<AdminContent />)

        expect(screen.getByText('Удалить контент по id')).toBeInTheDocument()
        const idInputs = screen.getAllByPlaceholderText('id')
        expect(idInputs.length).toBe(3) 
        expect(screen.getByText('Удалить')).toBeInTheDocument()

        expect(screen.getByText('Добавить фильм')).toBeInTheDocument()
        expect(screen.getByText('Добавить сериал')).toBeInTheDocument()

        expect(screen.getByText('Редактировать фильм')).toBeInTheDocument()
        expect(screen.getByText('Редактировать сериал')).toBeInTheDocument()
    })

    it('handles deleting content successfully', async () => {
        adminContentService.deleteContent.mockResolvedValue({
            response: { status: 200 },
            data: {},
        })

        render(<AdminContent />)

        const idInput = screen.getAllByPlaceholderText('id')[0]
        const deleteButton = screen.getByText('Удалить')

        fireEvent.change(idInput, { target: { value: '123' } })
        fireEvent.click(deleteButton)

        await waitFor(() => {
            expect(adminContentService.deleteContent).toHaveBeenCalledWith(123)
            expect(toast.success).toHaveBeenCalledWith('Успешно удалено', { position: 'bottom-center' })
        })
    })

    it('handles adding a movie', () => {
        render(<AdminContent />)

        const addMovieSection = screen.getByText('Добавить фильм')
        const addMovieArrow = addMovieSection.nextSibling

        fireEvent.click(addMovieArrow)

        expect(screen.getByTestId('add-movie-options')).toBeInTheDocument()

        fireEvent.click(addMovieArrow)

        expect(screen.queryByTestId('add-movie-options')).not.toBeInTheDocument()
    })

    it('handles adding a serial', () => {
        render(<AdminContent />)

        const addSerialSection = screen.getByText('Добавить сериал')
        const addSerialArrow = addSerialSection.nextSibling

        fireEvent.click(addSerialArrow)

        expect(screen.getByTestId('add-serial-options')).toBeInTheDocument()

        fireEvent.click(addSerialArrow)

        expect(screen.queryByTestId('add-serial-options')).not.toBeInTheDocument()
    })

    it('handles editing a movie successfully', async () => {
        const mockOptions = [{ key: 'value' }]
        adminContentService.getEditMovieContent.mockResolvedValue({
            response: { ok: true },
            data: mockOptions,
        })

        render(<AdminContent />)

        const editMovieIdInput = screen.getAllByPlaceholderText('id')[1]
        const editMovieButton = screen.getByText('Редактировать фильм').parentElement.querySelector('button')

        fireEvent.change(editMovieIdInput, { target: { value: '456' } })
        fireEvent.click(editMovieButton)

        await waitFor(() => {
            expect(adminContentService.getEditMovieContent).toHaveBeenCalledWith(456)
            expect(screen.getByTestId('edit-movie-options')).toBeInTheDocument()
            expect(screen.getByText(`EditMovieOptions: ${JSON.stringify(mockOptions)}`)).toBeInTheDocument()
        })
    })

    it('shows error toast when editing a movie fails', async () => {
        adminContentService.getEditMovieContent.mockResolvedValue({
            response: { ok: false },
            data: { message: 'Error fetching movie' },
        })

        render(<AdminContent />)

        const editMovieIdInput = screen.getAllByPlaceholderText('id')[1]
        const editMovieButton = screen.getByText('Редактировать фильм').parentElement.querySelector('button')

        fireEvent.change(editMovieIdInput, { target: { value: '789' } })
        fireEvent.click(editMovieButton)

        await waitFor(() => {
            expect(adminContentService.getEditMovieContent).toHaveBeenCalledWith(789)
            expect(toast.error).toHaveBeenCalledWith('Error fetching movie', { position: 'bottom-center' })
            expect(screen.queryByTestId('edit-movie-options')).not.toBeInTheDocument()
        })
    })

    it('handles stopping movie edit', async () => {
        const mockOptions = [{ key: 'value' }]
        adminContentService.getEditMovieContent.mockResolvedValue({
            response: { ok: true },
            data: mockOptions,
        })

        render(<AdminContent />)

        const editMovieIdInput = screen.getAllByPlaceholderText('id')[1]
        const editMovieButton = screen.getByText('Редактировать фильм').parentElement.querySelector('button')

        fireEvent.change(editMovieIdInput, { target: { value: '456' } })
        fireEvent.click(editMovieButton)

        await waitFor(() => {
            expect(screen.getByTestId('edit-movie-options')).toBeInTheDocument()
        })

        const stopButton = screen.getByText('Прекратить')
        fireEvent.click(stopButton)

        expect(screen.queryByTestId('edit-movie-options')).not.toBeInTheDocument()
    })

    it('handles editing a serial successfully', async () => {
        const mockOptions = [{ key: 'value' }]
        adminContentService.getEditSerialContent.mockResolvedValue({
            response: { ok: true },
            data: mockOptions,
        })

        render(<AdminContent />)

        const editSerialIdInput = screen.getAllByPlaceholderText('id')[2]
        const editSerialButton = screen.getByText('Редактировать сериал').parentElement.querySelector('button')

        fireEvent.change(editSerialIdInput, { target: { value: '321' } })
        fireEvent.click(editSerialButton)

        await waitFor(() => {
            expect(adminContentService.getEditSerialContent).toHaveBeenCalledWith(321)
            expect(screen.getByTestId('edit-serial-options')).toBeInTheDocument()
            expect(screen.getByText(`EditSerialOptions: ${JSON.stringify(mockOptions)}`)).toBeInTheDocument()
        })
    })

    it('shows error toast when editing a serial fails', async () => {
        adminContentService.getEditSerialContent.mockResolvedValue({
            response: { ok: false },
            data: { message: 'Error fetching serial' },
        })

        render(<AdminContent />)

        const editSerialIdInput = screen.getAllByPlaceholderText('id')[2]
        const editSerialButton = screen.getByText('Редактировать сериал').parentElement.querySelector('button')

        fireEvent.change(editSerialIdInput, { target: { value: '654' } })
        fireEvent.click(editSerialButton)

        await waitFor(() => {
            expect(adminContentService.getEditSerialContent).toHaveBeenCalledWith(654)
            expect(toast.error).toHaveBeenCalledWith('Error fetching serial', { position: 'bottom-center' })
            expect(screen.queryByTestId('edit-serial-options')).not.toBeInTheDocument()
        })
    })

    it('handles stopping serial edit', async () => {
        const mockOptions = [{ key: 'value' }]
        adminContentService.getEditSerialContent.mockResolvedValue({
            response: { ok: true },
            data: mockOptions,
        })

        render(<AdminContent />)

        const editSerialIdInput = screen.getAllByPlaceholderText('id')[2]
        const editSerialButton = screen.getByText('Редактировать сериал').parentElement.querySelector('button')

        fireEvent.change(editSerialIdInput, { target: { value: '321' } })
        fireEvent.click(editSerialButton)

        await waitFor(() => {
            expect(screen.getByTestId('edit-serial-options')).toBeInTheDocument()
        })

        const stopButton = screen.getByText('Прекратить')
        fireEvent.click(stopButton)

        expect(screen.queryByTestId('edit-serial-options')).not.toBeInTheDocument()
    })
})
