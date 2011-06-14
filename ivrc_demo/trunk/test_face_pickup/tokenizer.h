#pragma once

#include <string>  // string
#include <utility> // pair
template<typename StringT>
class tokenizer {
public:
typedef StringT string_type;
typedef typename string_type::size_type size_type;
typedef std::pair<size_type,size_type> range_type;

	// �R���X�g���N�^: ��؂蕶���̏W���������ɗ^����
	explicit tokenizer(const string_type& delim) : dlm_(delim) {}

	// �^����������̃g�[�N���������J�n����
	void start(const string_type& source) { src_ = source; reset(); }

	// start�ɗ^����������̃g�[�N���������ŏ�������Ȃ���
	void reset() { rng_.first = 0; rng_.second = 0; }

	// �g�[�N����؂�o���B�؂�o���g�[�N�����Ȃ����false��Ԃ�
	bool next() {
	  rng_.first = src_.find_first_not_of(dlm_, rng_.first+rng_.second);
	  if ( rng_.first == std::string::npos ) return false;
	  rng_.second = src_.find_first_of(dlm_, rng_.first);
	  if ( rng_.second == std::string::npos ) {
		rng_.second = src_.size() - rng_.first;
		return true;
	  }
	  rng_.second -= rng_.first;
	  return true;
	}

	// next�Ő؂�o���ꂽ�g�[�N����Ԃ�
	string_type token() const 
	  { return src_.substr(rng_.first,rng_.second); }

	// next�؂�o���ꂽ�g�[�N���̈ʒu�ƒ�����Ԃ�
	range_type range() const { return rng_; }

	// ��C�ɐ؂�o����OutputIterator�ɏ����o��
	template<typename OutputIterator>
	OutputIterator split(OutputIterator out)
	  { while ( next() ) *out++ = token(); return out; }
	template<typename OutputIterator>
	OutputIterator split(const string_type& source, OutputIterator out) 
	  { start(source); return split(out); }

	private:
	string_type src_; // �����ΏۂƂȂ镶����
	string_type dlm_; // ��؂蕶���̏W��
	range_type  rng_; // �g�[�N���̈ʒu�ƒ���
};
