import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/search/bloc/search_bloc.dart';
import 'package:netflix/ui/search/bloc/search_event.dart';
import 'package:netflix/ui/search/bloc/search_state.dart';
import 'package:netflix/utils/app_colors.dart';

class RatingFilter extends StatefulWidget {
  const RatingFilter({super.key});

  @override
  State<RatingFilter> createState() => _RatingFilterState();
}

class _RatingFilterState extends State<RatingFilter> {
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
        const Text('Рейтинг', style: TextStyle(color: AppColors.textWhite, fontSize: 18, fontWeight: FontWeight.bold)),
        BlocBuilder<SearchBloc, SearchState>(
          builder: (context, state) {
            final params = state.filterParams;
            _fromController.text = _formatRating(state.filterParams.ratingFrom);
            _toController.text = _formatRating(state.filterParams.ratingTo);
            return Row(
              spacing: 20,
              children: [
                Expanded(
                  child: _RatingInputField(
                    label: 'От',
                    controller: _fromController,
                    onChanged: (value) => _updateRating(
                      context,
                      ratingFrom: value,
                      ratingTo: params.ratingTo,
                    ),
                  ),
                ),
                Container(
                  color: AppColors.textWhite,
                  height: 1,
                  width: 20,
                ),
                Expanded(
                  child: _RatingInputField(
                    label: 'До',
                    controller: _toController,
                    onChanged: (value) => _updateRating(
                      context,
                      ratingFrom: params.ratingFrom,
                      ratingTo: value,
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

  void _updateRating(
      BuildContext context, {
        required double? ratingFrom,
        required double? ratingTo,
      }) {
    context.read<SearchBloc>().add(
      UpdateFilterParams(
            (params) => params.copyWith(ratingFrom: (ratingFrom, true), ratingTo: (ratingTo, true)),
      ),
    );
  }

  String _formatRating(double? value) {
    if (value == null) return '';
    return value == value.roundToDouble()
        ? value.toStringAsFixed(0)
        : value.toString();
  }
}

class _RatingInputField extends StatelessWidget {
  final String label;
  final TextEditingController controller;
  final ValueChanged<double?> onChanged;

  const _RatingInputField({
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
        labelStyle: const TextStyle(color: AppColors.textWhite, fontSize: 16),
        border: const OutlineInputBorder(
            borderSide: BorderSide(
              color: AppColors.textWhite,
            ),
            borderRadius: BorderRadius.all(Radius.circular(8))
        ),
        filled: true,
        fillColor: Colors.grey[800],
      ),
      style: const TextStyle(color: AppColors.textWhite, fontSize: 16),
      inputFormatters: [
        FilteringTextInputFormatter.digitsOnly,
      ],
      onChanged: (text) {
        onChanged(text.isEmpty ? null : double.parse(text));
      },
    );
  }
}
