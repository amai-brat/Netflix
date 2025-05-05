import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/search/bloc/search_bloc.dart';
import 'package:netflix/ui/search/bloc/search_event.dart';
import 'package:netflix/ui/search/bloc/search_state.dart';
import 'package:netflix/utils/app_colors.dart';

class YearFilter extends StatefulWidget {
  const YearFilter({super.key});

  @override
  State<YearFilter> createState() => _YearFilterState();
}

class _YearFilterState extends State<YearFilter> {
  late TextEditingController _fromController;
  late TextEditingController _toController;

  @override
  void initState() {
    super.initState();
    _fromController = TextEditingController();
    _toController = TextEditingController();
  }

  @override
  void dispose() {
    _fromController.dispose();
    _toController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      spacing: 10,
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        const Text('Год выпуска', style: TextStyle(color: AppColors.textWhite, fontSize: 18, fontWeight: FontWeight.bold)),
        BlocBuilder<SearchBloc, SearchState>(
          builder: (context, state) {
            final params = state.filterParams;
            _fromController.text = state.filterParams.yearFrom?.toString() ?? '';
            _toController.text = state.filterParams.yearTo?.toString() ?? '';
            return Row(
              spacing: 20,
              children: [
                Expanded(
                  child: _YearInputField(
                    label: 'От',
                    controller: _fromController,
                    onChanged: (value) => _updateYears(
                      context,
                      yearFrom: value,
                      yearTo: params.yearTo,
                    ),
                  ),
                ),
                Container(
                  color: AppColors.textWhite,
                  height: 1,
                  width: 20,
                ),
                Expanded(
                  child: _YearInputField(
                    label: 'До',
                    controller: _toController,
                    onChanged: (value) => _updateYears(
                      context,
                      yearFrom: params.yearFrom,
                      yearTo: value,
                    ),
                  ),
                ),
              ],
            );
          },
        ),
      ],
    );
  }

  void _updateYears(
      BuildContext context, {
        required int? yearFrom,
        required int? yearTo,
      }) {
    context.read<SearchBloc>().add(
      UpdateFilterParams(
            (params) => params.copyWith(yearFrom: (yearFrom, true), yearTo: (yearTo, true)),
      ),
    );
  }
}

class _YearInputField extends StatelessWidget {
  final String label;
  final TextEditingController controller;
  final ValueChanged<int?> onChanged;

  const _YearInputField({
    required this.label,
    required this.controller,
    required this.onChanged,
  });

  @override
  Widget build(BuildContext context) {
    return TextField(
      keyboardType: TextInputType.number,
      controller: controller,
      decoration: InputDecoration(
        labelText: label,
        labelStyle: const TextStyle(color: AppColors.textWhite),
        border: const OutlineInputBorder(
            borderSide: BorderSide(
              color: AppColors.textWhite,
            ),
            borderRadius: BorderRadius.all(Radius.circular(8))
        ),
        filled: true,
        fillColor: Colors.grey[800],
      ),
      style: const TextStyle(color: AppColors.textWhite),
      inputFormatters: [
        FilteringTextInputFormatter.digitsOnly
      ],
      onChanged: (text) {
        onChanged(text.isEmpty ? null : int.parse(text));
      },
    );
  }
}